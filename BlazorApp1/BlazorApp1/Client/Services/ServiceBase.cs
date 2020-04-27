using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorApp1.Client.Services
{
    public abstract class ServiceBase
    {
        public ServiceBase(HttpClient client)
        {
            Client = client;
        }

        protected HttpClient Client
        {
            get;
        }


        protected virtual async Task GetManyAsync<TExpected>(
            string path,
            Action<IEnumerable<TExpected>> actionOnSuccess,
            Action<ProblemDetails> actionOnProblem,
            CancellationToken cancellationToken = default)
            where TExpected : class
        {
            string json = await GetJsonAsync(path, cancellationToken);
            ProblemDetails? problem = Deserialize<ProblemDetails>(json);

            if (problem is { })
            {
                var taskOnProblem = TaskFromAction(actionOnProblem, problem);
                await taskOnProblem;
            }
            else
            {
                IEnumerable<TExpected>? expected = Deserialize<IEnumerable<TExpected>>(json);
                expected = EnsureNotNull(expected);

                var taskOnSuccess = TaskFromAction(actionOnSuccess, expected);
                await taskOnSuccess;
            }
        }

        private Task TaskFromAction<T>(Action<T> action, T state)
        {
            return new Task(ActionOfObjectFromActionOfT(action), state);
        }

        private Action<object> ActionOfObjectFromActionOfT<T>(Action<T> actionOfT)
        {
            return new Action<object>(o => actionOfT((T)o));
        }

        private IEnumerable<T> EnsureNotNull<T>(IEnumerable<T>? enumerable)
        {
            if (enumerable is null)
            {
                enumerable = new List<T>();
            }

            return enumerable;
        }

        private async Task<string> GetJsonAsync(string path, CancellationToken cancellationToken = default)
        {
            var response = await Client.GetAsync(path, cancellationToken);
            return await response.Content.ReadAsStringAsync();
        }


        private T? Deserialize<T>(string json)
            where T : class
        {
            try
            {
                return JsonSerializer.Deserialize<T>(json, null);
            }
            catch (JsonException)
            {
                return default;
            }
        }
    }
}
