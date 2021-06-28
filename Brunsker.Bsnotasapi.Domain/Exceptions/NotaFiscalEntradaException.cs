using Brunsker.Bsnotas.Domain.Exceptions;
using System.Runtime.Serialization;

namespace Brunsker.Bsnotas.Domain.Adapters
{
    public class NotaFiscalEntradaException : CoreException<NotaFiscalEntradaCoreError>
    {
        public NotaFiscalEntradaException(string message)
            : base(message)
        {
        }

        public NotaFiscalEntradaException(NotaFiscalEntradaCoreError notaFiscalEntradaCoreError)
        {
            AddError(notaFiscalEntradaCoreError);
        }

        protected NotaFiscalEntradaException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override string Key => "FuncaoCoreException";
    }

    public class NotaFiscalEntradaCoreError : CoreError
    {

        protected NotaFiscalEntradaCoreError(string key, string message) : base(key, message)
        {
        }
    }
}
