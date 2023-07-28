using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using Microsoft.ML;
using TiVi.MLService.Model;
using TiVi.MLService.Model.Response;
using TiVi.MLService.Models.Base;
using TiVi.MLService.Services.Interfaces;
using TiVi.MLService.Utilities;

namespace TiVi.MLService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieRecommenderController : ControllerBase
    {
        private readonly PredictionEnginePool<MovieRating, MovieRatingPrediction> _model;
        private readonly IUserCatalogMovieService _movieService;

        public MovieRecommenderController(PredictionEnginePool<MovieRating, MovieRatingPrediction> model, IUserCatalogMovieService movieService)
        {
            _model = model;
            _movieService = movieService;
        }

        [Authorize]
        [HttpPost]
        [Route("GetRecommenderScore")]
        public BaseJsonResponse<List<RecommenderScoreResponse>> GetRecommenderScore()
        {
            List<RecommenderScoreResponse> ratings = new List<RecommenderScoreResponse>();
            var result = new BaseJsonResponse<List<RecommenderScoreResponse>>();

            MovieRatingPrediction prediction = null;
            foreach (var movie in _movieService.GetTrendingMovies())
            {
                // Call the Rating Prediction for each movie prediction
                prediction = _model.Predict(new MovieRating
                {
                    userId = ClaimIdentity.CurrentUserId.ToString(),
                    movieId = movie.MovieId.ToString()
                });

                // Normalize the prediction scores for the "ratings" b/w 0 - 100
                float percentage;
                Sigmoid(prediction.Score, out percentage);
                float normalizedscore = percentage;

                // Add the score for recommendation of each movie in the trending movie list
                ratings.Add(new RecommenderScoreResponse() { MovieId = movie.MovieId, NormalizedScore = normalizedscore });
            }

            result.is_success = true;
            result.data = ratings;
            return result;
        }

        private bool Sigmoid(float x, out float percentage)
        {
             percentage = (float)(100 / (1 + Math.Exp(-x)));

            return true;
        }
    }
}
