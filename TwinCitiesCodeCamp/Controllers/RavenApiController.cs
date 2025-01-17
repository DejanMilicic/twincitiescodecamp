﻿using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using TwinCitiesCodeCamp.Data;

namespace TwinCitiesCodeCamp.Controllers
{
    /// <summary>
    /// Base class for API controllers that use the database.
    /// </summary>
    public abstract class RavenApiController : ApiController
    {
        public async override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            using (DbSession)
            {
                var result = await base.ExecuteAsync(controllerContext, cancellationToken);
                await DbSession.SaveChangesAsync();

                return result;
            }
        }

        //added to allow getting db data was throwing exception
        //wasn't sure the proper way to do this so will need to be redone.
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            DbSession = RavenContext.Db.OpenAsyncSession();
        }

        public IAsyncDocumentSession DbSession { get; private set; }
    }
}
