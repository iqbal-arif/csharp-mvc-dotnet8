﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Retail.DataAccess.Data;
using Retail.DataAccess.Repository.IRepository;
using Retail.Models;

namespace Retail.DataAccess.Repository
{
    public class CompanyRepository: Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _db;

        public CompanyRepository(ApplicationDbContext db)
            :base(db) 
        {
            _db = db;
        }

        
        public void Update(Category obj)
        {
            _db.Categories.Update(obj);
        }
    }
}
