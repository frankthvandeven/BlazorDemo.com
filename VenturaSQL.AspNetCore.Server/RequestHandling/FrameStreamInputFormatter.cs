using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace VenturaSQL.AspNetCore.Server.RequestHandling
{

    public class FrameStreamInputFormatter : InputFormatter
    {
        const int bufferLength = 16384;

        public FrameStreamInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/venturasql"));
        }

        public async override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            int payLoadLength = (int)context.HttpContext.Request.ContentLength;

            using (MemoryStream ms = new MemoryStream(payLoadLength))
            {
                await context.HttpContext.Request.Body.CopyToAsync(ms);

                byte[] buffer = ms.GetBuffer();

                if( buffer.Length != payLoadLength)
                {
                    buffer = ms.ToArray();
                }

                return await InputFormatterResult.SuccessAsync(buffer);
            }
        }

        protected override bool CanReadType(Type type)
        {
            if (type == typeof(byte[]))
                return true;
            else
                return false;
        }
    }
}
