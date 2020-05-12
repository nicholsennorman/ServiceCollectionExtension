using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ServiceCollectionExtension.Configuration.Cache;

namespace ServiceCollectionExtension.Sample.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ICache _cache;
        public DateTime ApplicationTime { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ICache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public void OnGet()
        {
            ApplicationTime = _cache.Get<DateTime>("DateTime");
        }
    }
}
