using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using D3K.Diagnostics.Core.Log;

namespace D3K.Diagnostics.Log4net.Demo.ConsoleApp
{
    public class LoggingService : IService
    {
        readonly IService _service;
        readonly ILogger _logger;

        readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

        public LoggingService(IService service, ILogger logger)
        {
            _service = service ?? throw new ArgumentNullException();
            _logger = logger ?? throw new ArgumentNullException();
        }

        public DoWorkResult DoWork(DoWorkArgs args)
        {            
            var argsJson = JsonConvert.SerializeObject(new { Name = nameof(args), Value = args}, _jsonSettings);

            _logger.Debug($">>{nameof(IService)}, {nameof(DoWork)}>> InputArgs: [{argsJson}]");

            var res = _service.DoWork(args);

            var resJson = JsonConvert.SerializeObject(res, _jsonSettings);

            _logger.Debug($">>{nameof(IService)}, {nameof(DoWork)}>> ReturnValue: {resJson}");

            return res;
        }
    }
}
