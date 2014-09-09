using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace SimulateMVC
{
    public class HomeController:IController
    {
        /// <summary>
        /// 方法列表
        /// </summary>
        private static IList<MethodInfo> AllAction = new List<MethodInfo>();
        /// <summary>
        /// 类中全局的HttpContext对象
        /// </summary>
        private HttpContext _context = null;

        /// <summary>
        /// 类的执行方法
        /// </summary>
        /// <param name="context">传入的上下文对象</param>
        public void Execute(HttpContext context)
        {
            //获取Url请求参数
            string action = context.Request.QueryString["a"];
            //接收传入的上下文对象
            _context = context;

            //获取本类中的所有实例方法
            AllAction = this.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).ToList<MethodInfo>();            

            //遍历为方法对象赋值
            //MethodInfo method = null;
            //foreach (var item in AllAction)
            //{
            //    if (item.Name.Equals(action, StringComparison.CurrentCultureIgnoreCase))
            //    {
            //        method = this.GetType().GetMethod(item.Name,BindingFlags.NonPublic | BindingFlags.Instance);
            //    }
            //}

            //获取参数请求的方法
            MethodInfo method = AllAction.Where(a => a.Name.Equals(action, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            //方法不为空则调用方法
            if (method == null)
            {
                throw new HttpException(404, "Not Found");
            }
            method.Invoke(this, null);
        }

        private void Index()
        {
            _context.Response.Write(1);
        }
    }
}