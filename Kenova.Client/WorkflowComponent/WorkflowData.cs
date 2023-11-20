using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workflows.Client.Components;

// This class will be serialized to JSON. The Blazor JSON serializer converts member names to camelCase.
//
// Use camelCase for all properties.


public class WorkflowData
{
    public string id { get; set; }

    public List<WorkflowItem> items { get; set; } = new();

}

public class WorkflowItem
{
    public string id { get; set; }
    public double x { get; set; }
    public double y { get; set; }
    public bool isDragging { get; set; }

    public List<WorkflowConnector> inConnectors { get; set; } = new();
    public List<WorkflowConnector> outConnectors { get; set; } = new();
}

public class WorkflowConnector
{
    public double x { get; set; }
    public double y { get; set; }

    public bool connected { get; set; }
    public string connectedTo { get; set; }  
    public int connectedIndex { get; set; }
}
