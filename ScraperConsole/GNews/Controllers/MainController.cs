using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using GNews.Models;
using GraphQL;
using GraphQL.Types;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using GraphQL.Validation;

using System.IO;
using System.Net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
namespace GNews.Controllers
{
    [Route("graphql")]
    [ApiController]
    public class MainController : Controller
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;

        public MainController(ISchema schema, IDocumentExecuter documentExecuter)
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
        }
        [HttpOptions]
        public ActionResult Options()
        {
            return Ok("Hello World!");
        }
        [HttpGet]
        public ActionResult Get()
        {
            return Ok("Hello World!");
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            //if (query.Variables == null)
            //{
            //    return BadRequest();
            //}

            var inputs = query.Variables == null ? null : JsonConvert.DeserializeObject<Dictionary<string, object>>(query.Variables.ToString()).ToInputs();
            //var inputs = query.Variables.ToInputs();
           //var inputs = JsonConvert.DeserializeObject<Dictionary<string, object>>(query.Variables.ToString()).ToInputs();
            var executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = query.Query,
                Inputs = inputs
            };

            var result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

            if (result.Errors?.Count > 0)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}
