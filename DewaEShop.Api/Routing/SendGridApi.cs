using DewaEShop.Contract;
using DewaEShop.SendGrid;
using Microsoft.AspNetCore.Mvc;

namespace DewaEShop.Routing
{
    public static class SendGridApi
    {
        public static IEndpointRouteBuilder MapSendGridApi(this IEndpointRouteBuilder endpoints)
        {
            var baseAPI = new BaseApi(endpoints);
            var sendGridRoutes = baseAPI.CreateRouteGroup(endpoints, "sendgrid").MapToApiVersion(1);

            sendGridRoutes.MapPost("send-template-test", async (ISendGridEmailSender emailSender, [FromBody] SendEmailRequestDto request) =>
            {
                if (string.IsNullOrWhiteSpace(request.ToEmail))
                {
                    var errorResponse = new ApiResponseDto<object>(false, "Email is empty");
                    return Results.NotFound(errorResponse);
                }

                if (string.IsNullOrWhiteSpace(request.Subject))
                {
                    var errorResponse = new ApiResponseDto<object>(false, "Subject is empty");
                    return Results.NotFound(errorResponse);
                }

                if (string.IsNullOrWhiteSpace(request.TemplateId))
                {
                    var errorResponse = new ApiResponseDto<object>(false, "Template Id is empty");
                    return Results.NotFound(errorResponse);
                }

                var keyValuePair = new EmailDataDto
                {
                    EmailData = new Dictionary<string, string>
                    {
                        { "SUBJECT", request.Subject ?? "" },
                        { "FULLNAME", "John Pantau" },
                        { "LOGO", "https://dev-portal.invcar.com/assets/InvCar-Dark-Horizontal.png" }
                    }
                };

                await emailSender.SendEmailWithTemplateAsync(request.ToEmail, request.TemplateId, keyValuePair.EmailData);

                var result = new ApiResponseDto<object>(true, "Template email sent successfully");
                return Results.Ok(result);
            });

            return endpoints;
        }
    }
}
