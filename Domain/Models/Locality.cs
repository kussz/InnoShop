﻿using System;
using System.Collections.Generic;

namespace InnoShop.Domain.Models;

public partial class Locality
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
