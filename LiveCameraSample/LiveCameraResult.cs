namespace LiveCameraSample
{
    // Class to hold all possible result types. 
    public class LiveCameraResult
    {
        public Microsoft.ProjectOxford.Face.Contract.Face[] Faces { get; set; } = null;
        public Microsoft.ProjectOxford.Face.Contract.Person Person { get; set; } = null;
    }
}
