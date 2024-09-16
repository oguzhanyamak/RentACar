using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Core.Application.Pipelines.Transaction;

public class TransactionScopeBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, ITransactionalRequest
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);
        TResponse response;

		try
		{
			response = await next();
			scope.Complete();
		}
		catch (Exception)
		{
			scope.Dispose();
			throw;
		}
		return response;
    }
}

/*
 ITransactionalRequest ile işaretlenen requestler pipeline a girip işlem sırasında hata oluşması durumunda o işlem sırasında yapılan varsa diğer işlemlerinde geri alınmasını sağlamaktadır.
 */
