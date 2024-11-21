﻿using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io;

namespace Ensek.Tests;

// ReSharper disable ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
#pragma warning disable CA8602

// TODO: Move to Test Util project
[ExcludeFromCodeCoverage]
public static class HtmlHelpers
{
    public static async Task<IHtmlDocument> GetDocumentAsync([NotNull] HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        IDocument document = await BrowsingContext.New()
            .OpenAsync(ResponseFactory, CancellationToken.None);
        return (IHtmlDocument)document;

        void ResponseFactory(VirtualResponse htmlResponse)
        {
            htmlResponse
                .Address(response.RequestMessage.RequestUri)
                .Status(response.StatusCode);

            MapHeaders(response.Headers);
            MapHeaders(response.Content.Headers);

            htmlResponse.Content(content);

            void MapHeaders(HttpHeaders headers)
            {
                foreach (var header in headers)
                {
                    foreach (var value in header.Value)
                    {
                        htmlResponse.Header(header.Key, value);
                    }
                }
            }
        }
    }
}