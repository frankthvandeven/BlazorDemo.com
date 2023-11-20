using Kenova.Client.Components;
using Kenova.Client.Util;
using System;
using System.Threading.Tasks;

namespace Kenova.Client
{
    public partial class LongRunningTask
    {
        public Exception Exception { get; private set; } = null;
        public bool Success { get; private set; } = false;

        /// <summary>
        /// Returns false if the action did throw an exception.
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="action">For example "Model.SaveCustomerTask" or "() => Model.SaveCustomerTask(options)" if you want to set parameters of the action.</param>
        /// <returns></returns>
        public async Task<bool> Run(string caption, Func<Task> action)
        {
            if (caption == null)
                throw new ArgumentNullException("caption");

            Exception = null;
            Success = false;

            bool result = false;

            await JavaScriptCaller.KNShowTaskrunnerAsync(caption);

            await Task.Delay(1);

            try
            {
                await action();
                result = true;
            }
            catch (Exception ex)
            {
                this.Exception = ex;
            }

            await JavaScriptCaller.KNHideTaskrunnerAsync();

            if (Exception != null)
            {
                await MessageBox.ShowAsync(caption, Exception.Message);
            }

            return result;
        }

        /// <summary>
        /// Returns a TResult type. TResult is default if the action throws an excepton.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="caption"></param>
        /// <param name="action">For example "Model.SaveCustomerTask" or "() => Model.SaveCustomerTask(options)" if you want to set parameters of the action.</param>
        /// <returns></returns>
        public async Task<TResult> Run<TResult>(string caption, Func<Task<TResult>> action)
        {
            if (caption == null)
                throw new ArgumentNullException("caption");

            Exception = null;
            Success = false;

            TResult result = default;

            await JavaScriptCaller.KNShowTaskrunnerAsync(caption);

            await Task.Delay(1);

            try
            {
                result = await action();
                Success = true;
            }
            catch (Exception ex)
            {
                this.Exception = ex;
            }

            await JavaScriptCaller.KNHideTaskrunnerAsync();

            if (Exception != null)
            {
                await MessageBox.ShowAsync(caption, Exception.Message);
            }

            return result;
        }

    }
}
