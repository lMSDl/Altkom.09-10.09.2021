﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configurations
{
    public class EditedDateTimeConfiguration<T> : IEntityTypeConfiguration<T> where T : class, IEditedDateTime
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.EditedDateTime).IsConcurrencyToken();
        }
    }
}
