using System;
using System.Threading.Tasks;

namespace Kenova.Client
{
    public partial class LongRunningTask
    {

        /// <summary>
        /// Returns false if the action did throw an exception.
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="action">For example "Model.SaveCustomerTask" or "() => Model.SaveCustomerTask(options)" if you want to set parameters of the action.</param>
        /// <returns></returns>
        public static async Task<bool> SimpleRun(string caption, Func<Task> action)
        {
            var lrt = new LongRunningTask();

            bool result = await lrt.Run(caption, action);

            return result;
        }

        /// <summary>
        /// Returns a TResult type.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="caption"></param>
        /// <param name="action">For example "Model.SaveCustomerTask" or "() => Model.SaveCustomerTask(options)" if you want to set parameters of the action.</param>
        /// <returns></returns>
        public static async Task<TResult> SimpleRun<TResult>(string caption, Func<Task<TResult>> action)
        {
            var lrt = new LongRunningTask();

            TResult result = await lrt.Run<TResult>(caption, action);

            return result;
        }


    }
}
