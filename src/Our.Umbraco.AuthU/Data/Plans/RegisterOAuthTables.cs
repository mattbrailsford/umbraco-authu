﻿using Our.Umbraco.AuthU.Data.Helpers;
using Our.Umbraco.AuthU.Data.Migrations.UmbracoDbOAuthClientStore;
using Our.Umbraco.AuthU.Data.Migrations.UmbracoDbOAuthRefreshTokenStore;
using Our.Umbraco.AuthU.Models;
using System;
using System.Linq;
using Umbraco.Core.Composing;
using Umbraco.Core.Migrations;

namespace Our.Umbraco.AuthU.Data.Migrations.Plans
{
    /// <summary>
    /// This is the plan that runs the AuthU Migrations
    /// </summary>
    internal class RegisterOAuthTables : MigrationPlan
    {
        /// <summary>
        /// Add the demo id if preferred
        /// </summary>
        /// <param name="CreateDemoClient"></param>
        public RegisterOAuthTables(bool CreateDemoClient = false) : base("AuthU")
        {
            if (!CreateDemoClient)
            {
                From(InitialState)
                .To<CreateAuthClientStoreTable>("CreateAuthClientStoreTable")
                .To<CreateAuthRefreshTokenStoreTable>("CreateAuthRefreshTokenStoreTable")
                .To<AddDeviceIdColumn>("AddDeviceIdColumn");
            }
            else
            {
                From(InitialState)
                .To<CreateAuthClientStoreTable>("CreateAuthClientStoreTable")
                .To<CreateAuthRefreshTokenStoreTable>("CreateAuthRefreshTokenStoreTable")
                .To<AddDeviceIdColumn>("AddDeviceIdColumn")
                .To<CreateDemoClient>("CreateDemoClient");
            }
        }

        /// <summary>
        /// Using the helper methid to get the AuthU Key 'Umbraco.Core.Upgrader.State+AuthU' Value
        /// </summary>
        public override string InitialState => AuthUKeyValueHelper.GetValue();
    }
}