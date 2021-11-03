// <copyright file="Style.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Kimchi_ssg
{
    internal class Style
    {
        public static string Def = @"<style>
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

        public static string DarkMode = @"<style>
                                *{
                                    background-color: #000000;
                                    color: #ffffff;
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

        public static string LightMode = @"<style>
                                *{
                                    background-color: #ffffff;
                                    color: #000000;
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
