namespace XVP.Infrastructure.Shared.Logging
{
    using System;
    using System.Web;

    using Elmah;

    public static class ElmahLogger
    {
        public static void LogError(Exception ex, string contextualMessage = null)
        {
            if (contextualMessage != null)
            {
                var annotatedException = new Exception(contextualMessage, ex);
                ErrorSignal.FromCurrentContext().Raise(annotatedException, HttpContext.Current);
            }
            else
            {
                ErrorSignal.FromCurrentContext().Raise(ex, HttpContext.Current);
            }
        }
    }
}