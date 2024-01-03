using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.SharedKernel
{
    public class Response
    {
        public bool IsError { get; set; } = false;
        public string Message { get; set; }
        public object Payload { get; set; }
        public List<Error> Errors { get; set; }
        public PagerInfo Pager { get; private set; } = null;

        public Response() { }

        public Response(IPagedList<object> pagedCollection)
        {
            Payload = pagedCollection;
            Message = ((IPagedList<object>)Payload).Any() ? "Registros encontrados" : "Nenhum registro encontrado";
            Pager = new PagerInfo(pagedCollection);
        }

        public Response(object obj)
        {
            Payload = obj;
        }

        public Response(string message, bool isError = false, object payload = null)
        {
            this.Message = message;
            this.IsError = isError;
            this.Payload = payload;
        }

        public Response(string message, bool isError = false, object payload = null, params string[] errors) : this(message, isError, payload)
        {
            this.Errors = errors.Select(x => new Error
            {
                Code = 0,
                Message = x
            }).ToList();
        }

        public Response SetPayload<T>(T payload)
        {
            this.Payload = payload;
            return this;
        }

        public Response SetPagerInfo(IPagedList pagedList)
        {
            this.Pager = new PagerInfo(pagedList);
            return this;
        }

        public Response SetPagerInfo(PagerInfo pagerInfo)
        {
            this.Pager = pagerInfo;
            return this;
        }

        public static explicit operator Task<object>(Response v)
        {
            throw new NotImplementedException();
        }
    }
}