using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ExpectedObjects;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ShoppingCartTests
{
    [TestClass]
    public class ProgramTests
    {
        private static List<Product> _Products { get; set; }
        private static Product _Product { get; set; }

        [ClassInitialize]
        public static void ProductInit(TestContext testContext)
        {
            _Product = new Product();
        }
        [TestMethod]
        public void 驗證Products_3筆一組取Cost總和_值依序為6_15_24_21()
        {
            var expected = new List<decimal> { 6, 15, 24, 21 };
            var actual = new List<decimal>()
            {
                _Product.GetSum(0, 3, product => product.Cost),
                _Product.GetSum(3, 3, product => product.Cost),
                _Product.GetSum(6, 3, product => product.Cost),
                _Product.GetSum(9, 3, product => product.Cost),
            };
            expected.ToExpectedObject().ShouldEqual(actual);
        }
        [TestMethod]
        public void 驗證Products_4筆一組取Revenue總和_值依序為50_66_60()
        {
            var expected = new List<decimal> { 50, 66, 60 };
            var actual = new List<decimal>
            {
                _Product.GetSum(0, 4, product => product.Revenue),
                _Product.GetSum(4, 4, product => product.Revenue),
                _Product.GetSum(8, 4, product => product.Revenue)
            };
            expected.ToExpectedObject().ShouldEqual(actual);
        }
        [TestMethod]
        public void 驗證Products_筆數輸入負數_會拋_ArgumentException()
        {
            Action act = () => _Product.GetSum(-11, 4, product => product.Revenue);
            act.ShouldThrow<ArgumentException>();
        }
        [TestMethod]
        public void 驗證Products_欄位若不存在_會拋_ArgumentException()
        {
            Action act = () => _Product.GetField("Test");
            act.ShouldThrow<ArgumentException>();
        }
        [TestMethod]
        public void 驗證Products_筆數若輸入為0_則傳回0()
        {
            decimal expected = _Product.GetSum(0, 4, product => product.Revenue);
            decimal actual = 0;
            expected.ToExpectedObject().ShouldEqual(actual);
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }
        public decimal Revenue { get; set; }
        public decimal SellPrice { get; set; }

        List<Product> Products => new List<Product>
        {
            new Product { Id = 1,  Cost = 1,  Revenue = 11, SellPrice = 21 },
            new Product { Id = 2,  Cost = 2,  Revenue = 12, SellPrice = 22 },
            new Product { Id = 3,  Cost = 3,  Revenue = 13, SellPrice = 23 },
            new Product { Id = 4,  Cost = 4,  Revenue = 14, SellPrice = 24 },
            new Product { Id = 5,  Cost = 5,  Revenue = 15, SellPrice = 25 },
            new Product { Id = 6,  Cost = 6,  Revenue = 16, SellPrice = 26 },
            new Product { Id = 7,  Cost = 7,  Revenue = 17, SellPrice = 27 },
            new Product { Id = 8,  Cost = 8,  Revenue = 18, SellPrice = 28 },
            new Product { Id = 9,  Cost = 9,  Revenue = 19, SellPrice = 29 },
            new Product { Id = 10, Cost = 10, Revenue = 20, SellPrice = 30 },
            new Product { Id = 11, Cost = 11, Revenue = 21, SellPrice = 31 }
        };

        public decimal GetSum(int count, int size, Func<Product, decimal> selector)
        {
            if (count < 0) throw new ArgumentException();
            return  Products.Skip(count).Take(size).Sum(selector);
        }

        public string GetField(string fieldName)
        {
            var field = typeof(Product).GetProperty(fieldName);
            if (field != null) return field.Name;
            throw new ArgumentException();
        }
    }
}