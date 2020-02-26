using System;
using System.Web;
using System.Web.SessionState;

namespace MF.Admin.BLL
{
    public class HttpHandler : IHttpModule, IRequiresSessionState, IReadOnlySessionState
    {
        public void Dispose()
        {

        }
        /// <summary>
        /// 执行顺序
        ///application_BeginRequest
        ///application_AuthenticateRequest
        ///application_AuthorizeRequest
        ///application_ResolveRequestCache
        ///application_AcquireRequestState
        ///application_PreRequestHandlerExecute
        ///------------------------------------------
        ///application_PostRequestHandlerExecute
        ///application_ReleaseRequestState
        ///application_EndRequest
        ///application_PreSendRequestHeaders
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            //指示请求处理开始
            context.BeginRequest += new EventHandler(application_BeginRequest);
            //在Http请求处理完成的时候触发
            context.EndRequest += new EventHandler(application_EndRequest);
            //在Http请求进入HttpHandler之前触发
            context.PreRequestHandlerExecute += new EventHandler(application_PreRequestHandlerExecute);

            //在Http请求进入HttpHandler之后触发
            context.PostRequestHandlerExecute += new EventHandler(application_PostRequestHandlerExecute);
            //存储Session状态时候触发
            context.ReleaseRequestState += new EventHandler(application_ReleaseRequestState);
            //加载初始化Session时候触发
            context.AcquireRequestState += new EventHandler(application_AcquireRequestState);
            //封装请求身份验证过程
            context.AuthenticateRequest += new EventHandler(application_AuthenticateRequest);
            //封装检查是否能利用以前缓存的输出页面处理请求的过程
            context.AuthorizeRequest += new EventHandler(application_AuthorizeRequest);
            //从缓存中得到相应时候触发
            context.ResolveRequestCache += new EventHandler(application_ResolveRequestCache);
            //在向客户端发送Header之前触发
            context.PreSendRequestHeaders += new EventHandler(application_PreSendRequestHeaders);
            //在向客户端发送内容之前触发
            context.PreSendRequestContent += new EventHandler(application_PreSendRequestContent);

            context.PostMapRequestHandler += new EventHandler(application_PostMapRequestHandler);
        }
        void application_PostMapRequestHandler(object sender, EventArgs e)
        {

        }

        void application_PreSendRequestContent(object sender, EventArgs e)
        {

        }
        void application_PreSendRequestHeaders(object sender, EventArgs e)
        {

        }
        void application_ResolveRequestCache(object sender, EventArgs e)
        {

        }
        void application_AuthorizeRequest(object sender, EventArgs e)
        {

        }
        void application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        void application_AcquireRequestState(object sender, EventArgs e)
        {

        }
        void application_ReleaseRequestState(object sender, EventArgs e)
        {

        }
        void application_PostRequestHandlerExecute(object sender, EventArgs e)
        {
        }

        void application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            try
            {
                string path = app.Request.Url.PathAndQuery.Split('?')[0].ToLower();
                string[] loginPath = path.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in loginPath)
                {
                    if (p.Equals("m") && loginPath[loginPath.Length - 1].ToLower() != "ajax.ashx")//验证用户是否已登录
                    {
                        if (!AdminBLL.CheckIsLogin())
                        {
                            app.Response.Redirect("/");
                            break;
                        }
                    }
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                Base.WriteError("检测管理员是否登录时出错：", ex.Message);
                app.Response.Redirect("/");
            }
        }

        void application_EndRequest(object sender, EventArgs e)
        {

        }

        void application_BeginRequest(object sender, EventArgs e)
        {

            HttpApplication app = (HttpApplication)sender;
            System.Web.HttpRequest Request = app.Context.Request;
            System.Web.HttpResponse Response = app.Context.Response;


        }


    }
}
