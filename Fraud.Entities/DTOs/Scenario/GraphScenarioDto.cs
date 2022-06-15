using System.Collections.Generic;
using Fraud.Entities.DTOs.State;
using Newtonsoft.Json;

namespace Fraud.Entities.DTOs.Scenario
{
    public class GraphScenarioDto
    {
        public int ScenarioId { get; set; }
        public List<StateVertexDto> StateVertices { get; set; }
        public List<StateEdgeDto> StateEdges { get; set; }

        public int StateVerticesCount => StateVertices.Count;
        public int StateEdgesCount => StateEdges.Count;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}