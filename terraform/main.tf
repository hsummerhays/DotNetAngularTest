provider "aws" {
  region = "us-east-1"
}

# S3 Bucket for assets
resource "aws_s3_bucket" "app_assets" {
  bucket = "clean-arch-aws-app-assets"
}

# App Runner for the .NET API
resource "aws_apprunner_service" "api_service" {
  service_name = "clean-arch-api"

  source_configuration {
    image_repository {
      image_configuration {
        port = "8080"
      }
      image_identifier      = "public.ecr.aws/your-repo/clean-arch-api:latest"
      image_repository_type = "ECR_PUBLIC"
    }
    auto_deployments_enabled = true
  }
}

# Static Website Hosting for Angular (S3 + CloudFront)
resource "aws_s3_bucket" "frontend_bucket" {
  bucket = "clean-arch-aws-app-frontend"
}

resource "aws_s3_bucket_website_configuration" "frontend_config" {
  bucket = aws_s3_bucket.frontend_bucket.id

  index_document {
    suffix = "index.html"
  }
}
