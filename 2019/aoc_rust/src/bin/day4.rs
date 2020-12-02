fn main() {
    let min = 172_851;
    let max = 675_869;

    let mut test1_valid_count = 0;
    let mut test2_valid_count = 0;
    for value in min..max {
        let ones = value % 10;
        let tens = value % 100 / 10;
        let hunds = value % 1_000 / 100;
        let thous = value % 10_000 / 1_000;
        let ten_thous = value % 100_000 / 10_000;
        let hund_thous = value / 100_000;

        let values = [hund_thous, ten_thous, thous, hunds, tens, ones];
        let mut is_valid = true;
        let mut test1_has_double = false;
        let mut test2_has_double = false;
        for pos in 0..(values.len() - 1) {
            if values[pos] > values[pos + 1] {
                is_valid = false;
                break;
            }
            if values[pos] == values[pos + 1] {
                test1_has_double = true;
                if pos > 0 && values[pos - 1] == values[pos] {
                    continue;
                }
                if pos < 4 && values[pos + 2] == values[pos] {
                    continue;
                }

                test2_has_double = true;
            }
        }

        if is_valid {
            if test1_has_double {
                test1_valid_count += 1;
            }
            if test2_has_double {
                test2_valid_count += 1;
            }
        }
    }

    println!("part 1 {}", test1_valid_count);
    println!("part 2 {}", test2_valid_count);
}

#[cfg(test)]
mod tests {
    // Note this useful idiom: importing names from outer (for mod tests) scope.
    use super::*;

    #[test]
    fn test_day_1() {
        // assert_eq!(day_1_test(111111), true);
        // assert_eq!(day_1_test(223450), false);
        // assert_eq!(day_1_test(123789), false);
    }

    #[test]
    fn test_day_2() {
        // assert_eq!(day_2_test(112233), true, "112233");
        // assert_eq!(day_2_test(123444), false, "123444");
        // assert_eq!(day_2_test(111122), true, "111122");
    }
}
