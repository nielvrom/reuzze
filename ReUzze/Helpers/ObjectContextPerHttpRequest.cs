using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReUzze.Models;

namespace ReUzze.Helpers
{
        public static class ObjectContextPerHttpRequest
        {
            public static reuzzeCS2 Context
            {
                get
                {
                    string objectContextKey = HttpContext.Current.GetHashCode().ToString("x");
                    if (!HttpContext.Current.Items.Contains(objectContextKey))
                    {
                        HttpContext.Current.Items.Add(objectContextKey, new reuzzeCS2());
                    }
                    return HttpContext.Current.Items[objectContextKey] as reuzzeCS2;
                }
            }
        }
    }
