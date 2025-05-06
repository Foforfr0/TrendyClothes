using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers {
    public static class HttpResponses {
        public static ObjectResult InternalServerError (string ex) {
            return new ObjectResult (new {
                error = ex
            }) {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}
