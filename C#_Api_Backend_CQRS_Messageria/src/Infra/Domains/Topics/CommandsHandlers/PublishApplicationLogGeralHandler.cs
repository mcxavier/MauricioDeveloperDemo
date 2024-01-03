using Infra.EntitityConfigurations.Contexts;
using Core.SharedKernel;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Domains.Log;
using Utils;

namespace Infra.Domains.Topics.CommandsHandlers
{
    internal class PublishApplicationLogGeralHandler : IRequestHandler<PublishApplicationLogGeral, Response>
    {
        private readonly CoreContext _context;

        public PublishApplicationLogGeralHandler(CoreContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(PublishApplicationLogGeral request, CancellationToken cancellationToken)
        {
            try
            {
                await _context.LogsGerais.AddAsync(new LogGeral
                {
                    StoreCode = request.StoreCode,
                    TopicId = request.TopicId,
                    EntityId = request.EntityId,
                    ReferenceMessageId = request.ReferenceMessageId,
                    Type = request.Type,
                    Message = request.Message,
                    MessageJson = request.MessageJson,
                    CreatedAt = DateTimeBrazil.Now,
                    CreatedBy = "Linx.IO",
                    ModifiedAt = DateTimeBrazil.Now,
                    ModifiedBy = "Linx.IO"
                });

                await _context.SaveChangesAsync();

                return new Response("Log Gravado.", false);
            }
            catch (System.Exception ex)
            {
                return new Response("Erro ao gravar log: " + ex.Message, true);
            }
        }
    }
}
