using AutoMapper;
using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Infrastructure.Entities.Currencies;
using MoneyManager.Infrastructure.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Debts;
using MoneyManager.Application.DTO.Transactions;
using MoneyManager.Application.Interfaces.Debts;
using MoneyManager.Application.Interfaces.Transactions;
using MoneyManager.Infrastructure.Constants;
using MoneyManager.Infrastructure.Entities.Debts;

namespace MoneyManager.Application.Services.Debts
{
    public class DebtPaymentService: IDebtPaymentService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<DebtPayment> _debtPaymentRepo;
        private readonly IMapper _mapper;
        private readonly ITransactionsService _transactionService;

        public DebtPaymentService(IUnitOfWork uow, IMapper mapper, ITransactionsService transactionService)
        {
            _db = uow;
            _mapper = mapper;
            _debtPaymentRepo = uow.CreateRepository<DebtPayment>();
            _transactionService = transactionService;
        }

        public async Task<IEnumerable<DebtPaymentDto>> GetAll()
        {
            var debtPayments = await _debtPaymentRepo.GetAll();
            return _mapper.Map<IEnumerable<DebtPaymentDto>>(debtPayments);
        }

        public async Task<Guid> Add(DebtPaymentDto debtPaymentDto)
        {
            var debtPayment = _mapper.Map<DebtPayment>(debtPaymentDto);
            debtPayment.Id = Guid.NewGuid();

            var transactionId = await CreateTransaction(debtPaymentDto);
            debtPayment.TransactionId = transactionId;
            await _debtPaymentRepo.Add(debtPayment);
            await _db.Commit();

            return debtPayment.Id;
        }

        public async Task Update(DebtPaymentDto debtDto)
        {
            var transactionDto = await _transactionService.GetById(debtDto.TransactionId);

            if (transactionDto != null)
            {
                transactionDto.AccountId = debtDto.TargetAccountId;
                transactionDto.Date = debtDto.Date;
                transactionDto.MoneyQuantity = debtDto.Amount;

                await _transactionService.Update(transactionDto);
            }
            else
            {
                await CreateTransaction(debtDto);
            }

            var debtPayment = _mapper.Map<DebtPayment>(debtDto);
            _debtPaymentRepo.Update(debtPayment);
            await _db.Commit();
        }

        public async Task Delete(Guid id)
        {
            var debtPayment = await _debtPaymentRepo.GetById(id);

            if (debtPayment != null)
            {
                await _transactionService.Delete(debtPayment.TransactionId);
            }

            await _debtPaymentRepo.Delete(id);
            await _db.Commit();
        }

        private async Task<Guid> CreateTransaction(DebtPaymentDto debtPaymentDto)
        {
            var transactionId = Guid.NewGuid();
            await _transactionService.Add(new TransactionDTO()
            {
                Id = transactionId,
                AccountId = debtPaymentDto.TargetAccountId,
                Date = debtPaymentDto.Date,
                MoneyQuantity = debtPaymentDto.Amount,
                IsSystem = true,
                //TODO: add special name and type
                Name = "Debt",
                TransactionTypeId = TransactionTypeConstants.System
            });

            return transactionId;
        }
    }
}
