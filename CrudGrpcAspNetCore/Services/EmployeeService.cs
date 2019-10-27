using CrudGrpcAspNetCore.Infrastructures;
using CrudGrpcAspNetCore.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudGrpcAspNetCore.Services
{
    public class EmployeeService : EmployeeManage.EmployeeManageBase
    {
        private readonly ILogger<EmployeeService> _logger;
        private readonly SkyHRContext _db;

        public EmployeeService(
            ILogger<EmployeeService> logger,
            SkyHRContext db)
        {
            _logger = logger;
            _db = db;
        }



        /// <summary>
        /// Get All
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<Employees> GetAll(Empty request, ServerCallContext context)
        {
            var result = new Employees();
            result.Items.AddRange(_db.Employees.Select(m=> new EmployeeModel() 
            {
                Id = m.ID,
                FirstName = m.FirstName,
                LastName = m.LastName
            
            }));
            return Task.FromResult(result);
        }

        /// <summary>
        /// Get By ID
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<EmployeeModel> GetByID(EmployeeFilter request, ServerCallContext context)
        {
            if(request == null)
            {
                return null;
            }

            var obj = await _db.Employees.FindAsync(request.Id);
            
            if (obj == null)
            {
                return null;
            }

            return new EmployeeModel() 
            { 
                Id = obj.ID,
                FirstName = obj.FirstName,
                LastName = obj.LastName
            };
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<EmployeeModel> Insert(EmployeeModel request, ServerCallContext context)
        {
            if (request == null)
            {
                return null;
            }

            var dbModel = new Models.Employee()
            {
                FirstName = request.FirstName,
                LastName = request.LastName

            };

            _db.Employees.Add(dbModel);
            await _db.SaveChangesAsync();

            return new EmployeeModel()
            {
                Id = dbModel.ID,
                FirstName = dbModel.FirstName,
                LastName = dbModel.LastName
            };
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<EmployeeModel> Update(EmployeeModel request, ServerCallContext context)
        {
            if (request == null)
            {
                return null;
            }

            //if (id != dto.Id)
            //{
            //    return BadRequest();
            //}

            var dbModel = new Models.Employee()
            {
                ID = request.Id,
                FirstName = request.FirstName,
                LastName = request.LastName

            };

            var entity = await _db.Employees.FindAsync(request.Id);

            if (entity == null)
            {
                return null;
            }

            _db.Entry(entity).CurrentValues.SetValues(dbModel);
            await _db.SaveChangesAsync();

            return new EmployeeModel()
            {
                Id = dbModel.ID,
                FirstName = dbModel.FirstName,
                LastName = dbModel.LastName
            };
        }


        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<EmployeeModel> Delete(EmployeeFilter request, ServerCallContext context)
        {
            if (request == null)
            {
                return null;
            }

            var dbModel = await _db.Employees.FindAsync(request.Id);
            if (dbModel == null)
            {
                return null;
            }

            _db.Employees.Remove(dbModel);
            await _db.SaveChangesAsync();

            return new EmployeeModel()
            {
                Id = dbModel.ID,
                FirstName = dbModel.FirstName,
                LastName = dbModel.LastName
            };
        }
    }
}
