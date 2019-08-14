using System;
using System.Collections.Generic;
using FluentAssertions;
using RosComponentTesting;
using Xunit;

namespace RosComponentTestingTests
{
    public class TimesTests
    {
        public class TimesValidationTests
        {
            public static IEnumerable<object[]> Data => new List<object[]>
            {
                new[] {Times.Exactly(10)},
                new[] { Times.Once},
                new[] { Times.Never},
                new[] { Times.AtLeast(1)},
                new[] { Times.AtMost(1)},
                new[] { Times.Between(10, 20)},
            };
            
            [Theory]
            [MemberData(nameof(Data))]
            public void Validation_returns_false_for_values_lower_min_value(Times target)
            {
                var testValue = target.Min - 1;

                var validationResult = target.IsValid(testValue);

                validationResult.Should().BeFalse();
            }
            
            [Theory]
            [MemberData(nameof(Data))]
            public void Validation_returns_false_for_values_lager_max_value(Times target)
            {
                if (target.Max == UInt32.MaxValue)
                    return;
                
                var testValue = target.Max + 1;

                var validationResult = target.IsValid(testValue);

                validationResult.Should().BeFalse();
            }
            
            [Theory]
            [MemberData(nameof(Data))]
            public void Validation_returns_true_for_values_equal_min_value(Times target)
            {
                var testValue = target.Min;

                var validationResult = target.IsValid(testValue);

                validationResult.Should().BeTrue();
            }
            
            [Theory]
            [MemberData(nameof(Data))]
            public void Validation_returns_true_for_values_equal_max_value(Times target)
            {
                var testValue = target.Max;

                var validationResult = target.IsValid(testValue);

                validationResult.Should().BeTrue();
            }
            
            [Theory]
            [MemberData(nameof(Data))]
            public void Validation_returns_true_for_values_between_min_and_max(Times target)
            {
                var testValue = target.Min + 1;
                
                if (testValue > target.Max)
                    return;
                
                var validationResult = target.IsValid(testValue);

                validationResult.Should().BeTrue();
            }
            
        }

        public class TimesEqualityTests
        {
            [Fact]
            public void Times_with_same_min_and_max_are_equal()
            {
                var a = Times.Between(1, 3);
                var b = Times.Between(1, 3);

                a.Equals(b).Should().BeTrue();
                b.Equals(a).Should().BeTrue();
            }
            
            [Fact]
            public void Times_with_different_min_and_max_are_not_equal()
            {
                var a = Times.Between(1, 3);
                var b = Times.Between(2, 5);

                a.Equals(b).Should().BeFalse();
                b.Equals(a).Should().BeFalse();
            }
        }
        public class NeverTests
        {
            [Fact]
            public void Never_has_min_and_max_0()
            {
                var target = Times.Never;

                target.Should().NotBeNull();
                target.Min.Should().Be(0);
                target.Max.Should().Be(0);
            }
        }
        
        public class OnceTests
        {
            [Fact]
            public void Once_has_min_and_max_1()
            {
                var target = Times.Once;

                target.Should().NotBeNull();
                target.Min.Should().Be(1);
                target.Max.Should().Be(1);
            }
        }

        public class AtLeastTests
        {
            [Fact]
            public void AtLeast_has_max_set_to_uint_max()
            {
                var target = Times.AtLeast(15);

                target.Should().NotBeNull();
                target.Min.Should().Be(15);
                target.Max.Should().Be(uint.MaxValue);
            }

            [Theory]
            [InlineData(0)]
            [InlineData(1)]
            [InlineData(int.MaxValue)]
            public void AtLeast_can_have_any_positive_value(int value)
            {
                var target = Times.AtLeast(value);

                target.Should().NotBeNull();
                target.Min.Should().Be((uint) value);
            }

            [Fact]
            public void AtLeast_must_not_have_a_negative_value()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => Times.AtLeast(-10));
            }
        }

        public class AtMostTests
        {
            [Fact]
            public void AtMost_has_min_set_to_0()
            {
                var target = Times.AtMost(15);

                target.Should().NotBeNull();
                target.Min.Should().Be(0);
                target.Max.Should().Be(15);
            }
            

            [Theory]
            [InlineData(0)]
            [InlineData(1)]
            [InlineData(int.MaxValue)]
            public void AtMost_can_have_any_positive_value(int value)
            {
                var target = Times.AtMost(value);

                target.Should().NotBeNull();
                target.Max.Should().Be((uint) value);
            }

            [Fact]
            public void AtMost_must_not_have_a_negative_value()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => Times.AtMost(-10));
            }
        }

        public class ExactlyTests
        {
            [Fact]
            public void Exactly_has_equal_min_and_max_()
            {
                int value = 10;
                var target = Times.Exactly(value);

                target.Should().NotBeNull();
                target.Min.Should().Be((uint)value);
                target.Max.Should().Be((uint)value);
            }
            

            [Theory]
            [InlineData(0)]
            [InlineData(1)]
            [InlineData(int.MaxValue)]
            public void Exactly_can_have_any_positive_value(int value)
            {
                var target = Times.Exactly(value);

                target.Should().NotBeNull();
                target.Min.Should().Be((uint) value);
                target.Max.Should().Be((uint) value);
            }

            [Fact]
            public void Exactly_must_not_have_a_negative_value()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => Times.Exactly(-10));
            }
        }

        public class BetweenTests
        {
            [Fact]
            public void Min_value_must_not_be_greater_than_max_value()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => Times.Between(15, 10));
            }
            
            [Fact]
            public void Min_value_must_not_be_greater_than_max_value_ints()
            {
                const int min = 15;
                const int max = 10;
                
                Assert.Throws<ArgumentOutOfRangeException>(() => Times.Between(min, max));
            }

            [Fact]
            public void Min_value_must_not_be_negative()
            {
                const int min = -5;
                const int max = 10;
                
                Assert.Throws<ArgumentOutOfRangeException>(() => Times.Between(min, max));
            }

            [Fact]
            public void Max_value_must_not_be_negative()
            {
                const int min = 5;
                const int max = -10;
                
                Assert.Throws<ArgumentOutOfRangeException>(() => Times.Between(min, max));
            }
            
            [Fact]
            public void Min_and_Max_may_be_equal()
            {
                const int min = 5;
                const int max = 5;

                var target = Times.Between(min, max);

                target.Should().NotBeNull();
                target.Min.Should().Be(min);
                target.Max.Should().Be(max);
            }

            [Fact]
            public void Standard_between_has_min_smaller_than_max()
            {
                const int min = 10;
                const int max = 50;

                var target = Times.Between(min, max);

                target.Should().NotBeNull();
                target.Min.Should().Be(min);
                target.Max.Should().Be(max);
            }
        }
    }
}