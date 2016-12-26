using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBoot.Common
{
    public class Result
    {
        ResultStatus _status = ResultStatus.OK;
        public Result()
        {

        }

        public Result(ResultStatus status)
        {
            this._status = status;
        }

        public ResultStatus Status
        {
            get { return this._status; }
            set { this._status = value; }
        }

        public string Msg { get; set; }
        public Result(ResultStatus status, string msg)
        {
            this.Status = status;
            this.Msg = msg;
        }

        public static Result CreateResult(string msg = null)
        {
            Result result = new Result(ResultStatus.OK, msg);
            return result;
        }

        public static Result CreateResult(ResultStatus status, string msg = null)
        {
            Result result = new Result(status);
            result.Msg = msg;
            return result;
        }

        public static Result<T> CreateResult<T>(T data)
        {
            Result<T> result = CreateResult<T>(ResultStatus.OK, data);
            return result;
        }

        public static Result<T> CreateResult<T>(ResultStatus status)
        {
            Result<T> result = CreateResult<T>(status, default(T));
            return result;
        }
        public static Result<T> CreateResult<T>(ResultStatus status, T data)
        {
            Result<T> result = new Result<T>(status);
            result.Data = data;
            return result;
        }
    }
}
