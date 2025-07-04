# Orderlyze Sales API

This repository contains a sample ASP.NET Core project with automatic OpenAPI generation.

## Overview

This project demonstrates modern .NET development practices with OpenAPI integration.

## OpenAPI Generation

The `Microsoft.Extensions.ApiDescription.Server` package automatically creates an OpenAPI document every time the project is built. When running in development, the OpenAPI spec is available through Swagger UI and Scalar.

## Scalar UI Authentication

Scalar is configured to display an `Authorize` button for JWT bearer tokens. Supply a valid token to authenticate calls directly from the documentation UI.
