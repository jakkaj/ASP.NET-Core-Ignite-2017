using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace PrettyJsonApi
{
    public interface ISomeService
    {
        string Key { get; }
        string HeaderKey { get; set; }
    }

    public class SomeService : ISomeService
    {
        private readonly ApiKeySettings _settings;

        public SomeService(IOptions<ApiKeySettings> options)
        {
            _settings = options.Value;
        }

        public string Key => _settings.ApiKey;

        public string HeaderKey { get; set; }
    }
}
