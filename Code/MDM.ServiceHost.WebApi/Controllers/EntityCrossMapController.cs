﻿using EnergyTrading.Mdm.Messages.Services;
using MDM.ServiceHost.WebApi.Infrastructure.Exceptions;
using MDM.ServiceHost.WebApi.Infrastructure.Extensions;
using Microsoft.Practices.ServiceLocation;

namespace MDM.ServiceHost.WebApi.Controllers
{
    using System.Net;
    using System.Transactions;
    using System.Web.Http;

    using EnergyTrading.Mdm;
    using EnergyTrading.Mdm.Messages;
    using EnergyTrading.Mdm.Services;

    using MDM.ServiceHost.WebApi.Filters;
    using MDM.ServiceHost.WebApi.Infrastructure.Controllers;
    using MDM.ServiceHost.WebApi.Infrastructure.ETags;
    using MDM.ServiceHost.WebApi.Infrastructure.Results;

    using EnergyTrading.Mdm.Contracts;

    /// <summary>
    /// This controller handles requests relating to the cross-mapping functionality within MDM.
    /// </summary>
    public class EntityCrossMapController<TContract, TEntity> : BaseEntityController
        where TContract : class, IMdmEntity
        where TEntity : IEntity
    {
        protected IMdmService<TContract, TEntity> service;

        public EntityCrossMapController(IMdmService<TContract, TEntity> service, IServiceLocator serviceLocator)
            : base(serviceLocator)
        {
            this.service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="etag"></param>
        /// <returns></returns>
        [ETagChecking]
        public IHttpActionResult Get([IfNoneMatch] ETag etag)
        {
            return WebHandler(() =>
            {
                var request = MessageFactory.CrossMappingRequest(this.QueryParameters);
                request.Version = etag.ToVersion();

                ContractResponse<MappingResponse> response;
                using (var scope = new TransactionScope(TransactionScopeOption.Required, ReadOptions()))
                {
                    response = this.service.CrossMap(request);
                    scope.Complete();
                }

                if (response.IsValid)
                {
                    return new ResponseWithETag<MappingResponse>(this.Request, response.Contract, HttpStatusCode.OK,
                        response.Version);
                }

                if (response.Error.Type == ErrorType.Ambiguous)
                {
                    throw new MdmFaultException(new CrossMappingAmbiguosMappingHandler().Create(
                        typeof(TContract).Name, response.Error, request));
                }

                throw new MdmFaultException(new CrossMappingRequestFaultHandler().Create(typeof(TContract).Name,
                    response.Error, request));
            });
        }
    }
}
