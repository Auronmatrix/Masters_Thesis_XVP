namespace XVP.Presentation.MVC.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Microsoft.Azure.Documents;

    using XVP.Domain.Commands;
    using XVP.Domain.Commands.Commands;
    using XVP.Infrastructure.Query.Interfaces;
    using XVP.Infrastructure.Query.Services;
    using XVP.Infrastructure.Shared.Abstracts;
    using XVP.Infrastructure.Shared.Logging;
    using XVP.Presentation.MVC.Services;

    public abstract class ADocumentController<TDocument> : BaseMultiTenantController where TDocument : SerializableResource
    {
        private IDocumentQueryService<TDocument> documentQueryService;

        private ICloudQueueService queueService;

        protected ICloudQueueService QueueService
        {
            get
            {
                if (this.queueService == null)
                {
                    throw new InvalidOperationException("Document Queue Service not initialized");
                }

                return this.queueService;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.queueService = value;
            }
        }

        protected IDocumentQueryService<TDocument> DocumentQueryService
        {
            get
            {
                if (this.documentQueryService == null)
                {
                    throw new InvalidOperationException("Document Service not initialized");
                }

                return this.documentQueryService;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.documentQueryService = value;
            }
        }

        protected virtual IQueryable<TDocument> Query()
        {
            IQueryable<TDocument> result = null;
            try
            {
                result = this.DocumentQueryService.Query();
            }
            catch (HttpResponseException ex)
            {
                ElmahLogger.LogError(ex);
            }
            catch (Exception ex)
            {
                ElmahLogger.LogError(ex);
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            return result;
        }

        protected virtual SingleResult<TDocument> Lookup(string id)
        {
            try
            {
                return this.DocumentQueryService.Lookup(id);
            }
            catch (HttpResponseException ex)
            {
                ElmahLogger.LogError(ex);
                throw;
            }
            catch (Exception ex)
            {
                ElmahLogger.LogError(ex);
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        protected virtual List<TDocument> GetAll()
        {
            try
            {
                return this.DocumentQueryService.GetAll();
            }
            catch (HttpResponseException ex)
            {
                ElmahLogger.LogError(ex);
                throw;
            }
            catch (Exception ex)
            {
                ElmahLogger.LogError(ex);
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        protected virtual TDocument GetById(string id)
        {
            try
            {
                return this.DocumentQueryService.GetById(id);
            }
            catch (HttpResponseException ex)
            {
                ElmahLogger.LogError(ex);
                throw;
            }
            catch (Exception ex)
            {
                ElmahLogger.LogError(ex);
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        protected virtual T GetById<T>(string id) where T : SerializableResource
        {
            try
            {
               var service = new DocumentQueryService<T>();
               return service.GetById(id);
            }
            catch (HttpResponseException ex)
            {
                ElmahLogger.LogError(ex);
                throw;
            }
            catch (Exception ex)
            {
                ElmahLogger.LogError(ex);
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        protected virtual async Task InsertAsync(TDocument item)
        {
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            try
            {
                ICommand command = new InsertGenericObjectCommand<TDocument>(this.CurrentTenant.Name, item);
                await QueueService.AddMessageAsync(command);
            }
            catch (HttpResponseException ex)
            {
                ElmahLogger.LogError(ex);
                throw;
            }
            catch (Exception ex)
            {
                ElmahLogger.LogError(ex);

                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        protected virtual async Task ReplaceAsync(string id, TDocument item)
        {
            if (item == null || !this.ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

           try
            {
                ICommand command = new ReplaceGenericObjectCommand<TDocument>(this.CurrentTenant.Name, item);
                await QueueService.AddMessageAsync(command);
            }
            catch (HttpResponseException ex)
            {
                ElmahLogger.LogError(ex);
                throw;
            }
            catch (Exception ex)
            {
                ElmahLogger.LogError(ex);
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        protected virtual async Task DeleteAsync(string id)
        {
           try
            {
                ICommand command = new DeleteGenericObjectCommand<TDocument>(this.CurrentTenant.Name, id);
                await QueueService.AddMessageAsync(command);
            }
            catch (HttpResponseException ex)
            {
                ElmahLogger.LogError(ex);
                throw;
            }
            catch (Exception ex)
            {
                ElmahLogger.LogError(ex);
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
    }
}