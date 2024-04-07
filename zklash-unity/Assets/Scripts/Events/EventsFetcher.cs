using System;
using System.Collections.Generic;
using SimpleGraphQL;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class EventNodeData
{
    public string id;
    public string[] keys;
    public string[] data;
    public string createdAt;
    public string transactionHash;
}

[Serializable]
public class EventNode
{
    public EventNodeData node;
}

[Serializable]
public class EventsEdges
{
    public List<EventNode> edges;
}

[Serializable]
public class EventsQueryResponse
{
    public EventsEdges events;
}

public class EventsFetcher : MonoBehaviour
{
    private GraphQLClient client;

    // Initialize your GraphQL client
    void Start()
    {
        client = new GraphQLClient("http://localhost:8080/graphql");
    }

    // Method to fetch events once
    public async Task<List<EventNode>> FetchEventsOnce(string[] keys)
    {
        string formattedKeys = string.Join(",", Array.ConvertAll(keys, key => $"\"{key}\""));
        Debug.Log($"Formatted keys: {formattedKeys}");
        string query = $@"query {{ events(keys: [{formattedKeys}], first: 1000) {{ edges {{ node {{ id keys data createdAt transactionHash }} }} }} }}";

        var request = new Request
        {
            Query = query,
        };

        var responseType = new
        {
            events = new
            {
                edges = new[]
                {
                    new
                    {
                        node = new
                        {
                            id = "",
                            keys = new string[] { },
                            data = new string[] { },
                            createdAt = "",
                            transactionHash = ""
                        }
                    }
                }
            }
        };

        try
        {
            Debug.Log("Fetching events...");
            // Adjust this to match the actual signature and usage pattern of your GraphQL client
            var response = await client.Send(() => responseType, request);

            // Assuming Send populates responseType and doesn't return a new object
            // This part heavily depends on how your client's Send method is implemented
            Debug.Log("Response received.");
            if (response.Data != null && response.Data.events != null)
            {
                List<EventNode> eventNodes = new List<EventNode>();

                foreach (var edge in response.Data.events.edges)
                {
                    var node = edge.node;
                    eventNodes.Add(new EventNode
                    {
                        node = new EventNodeData
                        {
                            id = node.id,
                            keys = node.keys,
                            data = node.data,
                            createdAt = node.createdAt,
                            transactionHash = node.transactionHash
                        }
                    });
                }
                Debug.Log("Events fetched successfully.");
                return eventNodes; // Return the list of EventNode directly
            }
            else
            {
                Debug.Log("No events found.");
                return new List<EventNode>();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to fetch events: {ex.Message}");
            return new List<EventNode>();
        }
    }


    private void ParseEvents(List<EventNode> edges)
    {
        // Assuming you have your EventParser instance and its method ready to use
        var eventParser = new EventParser(); // EventParser needs to be adapted or implemented based on your needs
        foreach (var edge in edges)
        {
            var node = edge.node;
            // Assuming EventParser has a method to process each node
            eventParser.ProcessNode(node.id, node.keys, node.data, node.createdAt, node.transactionHash);
        }
    }
}
