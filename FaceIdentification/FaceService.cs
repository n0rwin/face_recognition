using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FaceIdentification
{
    public class FaceService
    {
        const string personGroupId = "df_employees";
        const string faceApiKey = "f44c08e915b849b09ca844768a71c372";
        private static FaceServiceClient faceServiceClient = new FaceServiceClient(faceApiKey, "https://westeurope.api.cognitive.microsoft.com/face/v1.0");

        public async static void CreatePersonGroup()
        {
            await faceServiceClient.CreatePersonGroupAsync(personGroupId, "dataformers employees");

            var alexander = await faceServiceClient.CreatePersonAsync(personGroupId, "alexander");
            var anna = await faceServiceClient.CreatePersonAsync(personGroupId, "anna");
            var bernhard = await faceServiceClient.CreatePersonAsync(personGroupId, "bernhard");
            var david = await faceServiceClient.CreatePersonAsync(personGroupId, "david");
            var johannes = await faceServiceClient.CreatePersonAsync(personGroupId, "johannes");

            var lisa = await faceServiceClient.CreatePersonAsync(personGroupId, "lisa");
            var martina = await faceServiceClient.CreatePersonAsync(personGroupId, "martina");
            var norbert = await faceServiceClient.CreatePersonAsync(personGroupId, "norbert");
            var rene = await faceServiceClient.CreatePersonAsync(personGroupId, "rene");
            var sabine = await faceServiceClient.CreatePersonAsync(personGroupId, "sabine");

            var sarah = await faceServiceClient.CreatePersonAsync(personGroupId, "sarah");
            var sebastian = await faceServiceClient.CreatePersonAsync(personGroupId, "sebastian");
            var stefan = await faceServiceClient.CreatePersonAsync(personGroupId, "stefan");
            var tom = await faceServiceClient.CreatePersonAsync(personGroupId, "tom");
            var wilfried = await faceServiceClient.CreatePersonAsync(personGroupId, "wilfried");

            var employees = new Dictionary<string, CreatePersonResult>()
            {
                {"alexander", alexander },
                {"anna", anna},
                {"bernhard", bernhard},
                {"david", david},
                {"johannes", johannes},
                {"lisa", lisa},
                {"martina", martina},
                {"norbert", norbert},
                {"rene", rene},
                {"sabine", sabine},
                {"sarah", sarah},
                {"sebastian", sebastian},
                {"stefan", stefan},
                {"tom", tom},
                {"wilfried", wilfried}
            };

            foreach (var dir in Directory.GetDirectories(@"C:\projects\FaceIdentification\FaceIdentification\faces"))
            {
                foreach (var file in Directory.GetFiles(dir))
                {
                    using (Stream s = File.OpenRead(file))
                    {
                        var directoryName = new DirectoryInfo(dir).Name;
                        await faceServiceClient.AddPersonFaceAsync(personGroupId, employees.First(x => x.Key == directoryName).Value.PersonId, s);
                    }
                    Thread.Sleep(100);
                }
            }

            await faceServiceClient.TrainPersonGroupAsync(personGroupId);
        }

        public static async void Recognize()
        {
            string testImageFile = @"C:\projects\FaceIdentification\FaceIdentification\faces\norbert\me_cv.JPG";

            using (Stream s = File.OpenRead(testImageFile))
            {
                var faces = await faceServiceClient.DetectAsync(s);
                var faceIds = faces.Select(face => face.FaceId).ToArray();

                var results = await faceServiceClient.IdentifyAsync(personGroupId, faceIds);
                foreach (var identifyResult in results)
                {
                    Console.WriteLine("Result of face: {0}", identifyResult.FaceId);
                    if (identifyResult.Candidates.Length == 0)
                    {
                        Console.WriteLine("No one identified");
                    }
                    else
                    {
                        // Get top 1 among all candidates returned
                        var candidateId = identifyResult.Candidates[0].PersonId;
                        var person = await faceServiceClient.GetPersonAsync(personGroupId, candidateId);
                        Console.WriteLine("Identified as {0}", person.Name);
                    }
                }
            }
        }
    }
}
