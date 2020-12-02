use aoc_rust::intcodes::IntcodeComputer;

fn main() {
    let intcode_computer = IntcodeComputer::from_file("day2_input.txt");
    part_one(&mut intcode_computer.clone());
    part_two(intcode_computer);
}

fn part_one(intcode_computer: &mut IntcodeComputer) {
    intcode_computer.restore_and_process(12, 2);
    let result = intcode_computer.get_at_index(0);
    println!("part 1 {}", result)
}

fn part_two(intcode_computer: IntcodeComputer) {
    let mut found = false;
    for noun in 1..100 {
        if found {
            break;
        }

        // For verb, could use a search algo to find the value
        for verb in 1..100 {
            let mut loop_comp = intcode_computer.clone();
            loop_comp.restore_and_process(noun, verb);
            let result = loop_comp.get_at_index(0);
            if result != 19690720 {
                continue;
            }

            found = true;
            println!("part 2 {}", 100 * noun + verb);
            break;
        }
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn intcode_test1() {
        let test = vec![1, 0, 0, 0, 99];
        let mut comp = IntcodeComputer::from_vec(test);
        let test_expected = vec![2, 0, 0, 0, 99];
        comp.process_intcodes();
        assert_eq!(comp.intcodes, test_expected);
    }

    #[test]
    fn intcode_test2() {
        let test = vec![2, 3, 0, 3, 99];
        let mut comp = IntcodeComputer::from_vec(test);
        let test_expected = vec![2, 3, 0, 6, 99];
        comp.process_intcodes();
        assert_eq!(comp.intcodes, test_expected);
    }

    #[test]
    fn intcode_test3() {
        let test = vec![2, 4, 4, 5, 99, 0];
        let mut comp = IntcodeComputer::from_vec(test);
        let test_expected = vec![2, 4, 4, 5, 99, 9801];
        comp.process_intcodes();
        assert_eq!(comp.intcodes, test_expected);
    }

    #[test]
    fn intcode_test4() {
        let test = vec![1, 1, 1, 4, 99, 5, 6, 0, 99];
        let mut comp = IntcodeComputer::from_vec(test);
        let test_expected = vec![30, 1, 1, 4, 2, 5, 6, 0, 99];
        comp.process_intcodes();
        assert_eq!(comp.intcodes, test_expected);
    }
}
