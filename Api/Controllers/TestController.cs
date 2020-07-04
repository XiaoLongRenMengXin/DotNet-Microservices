using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service;

namespace Api.Controllers
{
    /// <summary>
    /// 测试
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录</param>
        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 获取测试结果
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Get()
        {
            return $"show: {TestService.Show()} Showing: {TestService.Showing()}";
        }
    }
}
