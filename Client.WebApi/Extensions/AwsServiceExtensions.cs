using Amazon.Runtime;
using Amazon.S3;
using Client.WebApi.Configurations;

namespace Client.WebApi.Extensions;

public static class AwsServiceExtensions
{
    public static IServiceCollection AddAwsS3(this IServiceCollection services, IConfiguration configuration)
    {
        var awsSection = configuration.GetSection(AwsSettings.SectionName);
        services.Configure<AwsSettings>(awsSection);

        var settings = awsSection.Get<AwsSettings>()
                       ?? throw new InvalidOperationException("AWS configuration section is missing.");

        var awsOptions = configuration.GetAWSOptions();

        if (!string.IsNullOrEmpty(settings.ServiceURL))
        {
            // Configuration-driven setup for a simulator
            services.AddSingleton<IAmazonS3>(sp =>
            {
                var config = new AmazonS3Config
                {
                    ServiceURL = settings.ServiceURL,
                    ForcePathStyle = true,
                    AuthenticationRegion = settings.Region,
                };

                var credentials = awsOptions.Credentials ?? new BasicAWSCredentials("test", "test");

                return new AmazonS3Client(credentials, config);
            });
        }
        else
        {
            // Standard AWS configuration using profiles for production/staging
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonS3>();
        }

        return services;
    }
}

