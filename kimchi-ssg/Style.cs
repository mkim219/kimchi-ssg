﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kimchi_ssg
{
    class Style
    {
        public static string def = @"<style>
                                *{
                                    background-color: #9999FF;
                                 }
                                .container{
                                  display: flex;
                                  overflow: auto;
                                }
                                .left-nav{
                                    width: 20%;
                                    position: sticky;
                                    align-self: flex-start;
                                    top: 0;
                                }

                                .contents { 
                                      color: #FFFFFF;
                                      width: 50%;
                                      top: 0;
                                      bottom: 0;
                                      left: 0;
                                      right: 0;
                                  }

                                ul {
                                  list-style-type: none;
                                  margin: 0;
                                  padding: 5px;
                                }

                                li a {
                                    display: block;
                                    text-decoration: none;
                                    padding: 5px;
                               }
                               </style>";
    }
}
