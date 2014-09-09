using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SimulateMVC
{
    /// <summary>
    /// Index 的摘要说明
    /// </summary>
    public class Index : IHttpHandler
    {
        /// <summary>
        /// 程序集中所有控制器类型的静态列表
        /// </summary>
        private static readonly IList<Type> AllControllerTypes = new List<Type>();

        /// <summary>
        /// 静态构造函数，获得程序集中所有实现了IController接口、公开的、非抽象的类型
        /// </summary>
        static Index()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            AllControllerTypes = types.Where(t => typeof(IController).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass && t.IsPublic).ToList<Type>();

        }

        /// <summary>
        /// 一般处理程序的PR方法
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            //获取Url请求参数
            var controllerName = context.Request.QueryString["c"] ?? string.Empty;

            //声明一个实现了IController接口的对象            
            IController controller = null;

            //遍历存放程序集中所有类的列表，如果请求参数符合列表中的类，则创建该类的实例
            foreach (var item in AllControllerTypes)
            {
                if (item.Name.Equals(controllerName + "controller", StringComparison.CurrentCultureIgnoreCase))
                {
                    controller = Activator.CreateInstance(item) as IController;
                }
            }

            //如果类实例不存在，抛出403错误，否则执行请求的类
            if (controller == null)
            {
                throw new HttpException(404, "Not Found");
            }            
            controller.Execute(context);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}