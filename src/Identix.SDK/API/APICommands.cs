using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identix.SDK.Readers.Entities
{
    class APICommands
    {
        public string name { get; set; }
        public string item { get; set; }
        public string request { get; set; }
        public string method { get; set; }
        public IEnumerable<string> header { get; set; }
		
        								"method": "GET",
								"header": [],
								"url": {
									"raw": "http://{{MINIPAD_IP}}:{{MINIPAD_PORT}}/config/data_output/heartbeat",
									"protocol": "http",
									"host": [
										"{{MINIPAD_IP}}"
									],
									"port": "{{MINIPAD_PORT}}",
									"path": [
										"config",
										"data_output",
										"heartbeat"
									]
	}
},
							"response": []
    }
}
