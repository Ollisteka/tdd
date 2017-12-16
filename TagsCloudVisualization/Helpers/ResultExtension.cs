using System;
using CSharpFunctionalExtensions;

namespace TagsCloudVisualization
{
	public static class ResultExtension
	{
		public static Result<T> Of<T>(Func<T> f, string error = null)
		{
			try
			{
				return Result.Ok(f());
			}
			catch (Exception e)
			{
				return Result.Fail<T>(error ?? e.Message);
			}
		}

		public static Result<TInput> ReplaceError<TInput>(
			this Result<TInput> input,
			Func<string, string> replaceError)
		{
			return input.IsSuccess
				? input
				: Result.Fail<TInput>(replaceError(input.Error));
		}

		public static Result<TInput> RefineError<TInput>(
			this Result<TInput> input,
			string errorMessage)
		{
			return input.ReplaceError(err => errorMessage + err);
		}
	}
}