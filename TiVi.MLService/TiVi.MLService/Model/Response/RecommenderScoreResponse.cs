namespace TiVi.MLService.Model.Response
{
    public class RecommenderScoreResponse
    {
        public int MovieId { get; set; }
        public float NormalizedScore { get; set; }
    }
}
