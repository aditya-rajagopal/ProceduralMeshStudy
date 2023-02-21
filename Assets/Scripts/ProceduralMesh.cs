using ProceduralMeshes;
using ProceduralMeshes.Generators;
using ProceduralMeshes.Streams;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralMesh : MonoBehaviour {

    public enum StreamType { Single, Multi }

    public enum GeneratorType {SquareGrid}

    public static ScheduleDelegate[,] meshJobs = {
        {
            MeshJob<SquareGrid, SingleStream>.ScheduleParallel,
            MeshJob<SquareGrid, MultiStream>.ScheduleParallel
        }
    };

    [SerializeField]
    StreamType streamType = StreamType.Single;

    [SerializeField]
    GeneratorType generatorType = GeneratorType.SquareGrid;

    Mesh mesh;

    private void Awake() {
        mesh = new Mesh {
            name = "Procedural Mesh"
        };

        GenerateMesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void GenerateMesh() {
       Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1); // we only want 1 mesh
       Mesh.MeshData meshData = meshDataArray[0];

       meshJobs[(int)generatorType, (int)streamType](mesh, meshData, default).Complete(); // default job handel and then when the job handel returns we wait for it to complete.

       Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);


    }

}