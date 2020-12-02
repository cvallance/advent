use aoc_rust::intcodes;
use intcodes::IntcodeComputer;

fn main() {
    let mut comp1 = IntcodeComputer::from_file("day5_input.txt");
    let mut comp2 = comp1.clone();
    comp1.inputs.push_back(1);
    comp1.process_intcodes();
    println!("part 1 {}", comp1.outputs.pop_back().unwrap());

    comp2.inputs.push_back(5);
    comp2.process_intcodes();
    println!("part 2 {}", comp2.outputs.pop_front().unwrap());
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn comp_echos_back_input() {
        let test = vec![3, 0, 4, 0, 99];
        let base_comp = IntcodeComputer::from_vec(test);

        test_input_output(&base_comp, 11, 11);
        test_input_output(&base_comp, -423, -423);
    }

    #[test]
    fn immediate_and_position() {
        let test = vec![1002, 4, 3, 4, 33];
        let mut comp = IntcodeComputer::from_vec(test);
        let test_expected = vec![1002, 4, 3, 4, 99];
        comp.process_intcodes();
        assert_eq!(comp.intcodes, test_expected);
    }

    #[test]
    fn immediate_and_position_negative() {
        let test = vec![1101, 100, -1, 4, 0];
        let mut comp = IntcodeComputer::from_vec(test);
        let test_expected = vec![1101, 100, -1, 4, 99];
        comp.process_intcodes();
        assert_eq!(comp.intcodes, test_expected);
    }

    #[test]
    fn op5_jump_if_true() {
        // Opcode 5 - if the first parameter is non-zero, it sets the instruction pointer to the value from the second parameter.
        // Positional mode
        let test = vec![5, 2, 3, 5, -1, 4, 0, 99];
        let comp = IntcodeComputer::from_vec(test);
        test_output(&comp, 5);
        // Immediate mode
        let test = vec![1105, 3, 5, -1, -1, 4, 0, 99];
        let comp = IntcodeComputer::from_vec(test);
        test_output(&comp, 1105);

        // Otherwise, it does nothing
        // Positional mode
        let test = vec![5, 2, 0, 4, 0, 99];
        let comp = IntcodeComputer::from_vec(test);
        test_output(&comp, 5);
        // Immediate mode
        let test = vec![1105, 0, -1, 4, 0, 99];
        let comp = IntcodeComputer::from_vec(test);
        test_output(&comp, 1105);
    }

    #[test]
    fn op6_jump_if_false() {
        // Opcode 6 - if the first parameter is zero, it sets the instruction pointer to the value from the second parameter.
        // Positional mode
        let test = vec![6, 2, 0, -1, -1, -1, 4, 0, 99];
        let comp = IntcodeComputer::from_vec(test);
        test_output(&comp, 6);
        // Immediate mode
        let test = vec![1106, 0, 6, -1, -1, -1, 4, 0, 99];
        let comp = IntcodeComputer::from_vec(test);
        test_output(&comp, 1106);

        // Otherwise, it does nothing
        // Positional mode
        let test = vec![6, 2, 100, 4, 0, 99];
        let comp = IntcodeComputer::from_vec(test);
        test_output(&comp, 6);
        // Immediate mode
        let test = vec![1106, 100, 0, 4, 0, 99];
        let comp = IntcodeComputer::from_vec(test);
        test_output(&comp, 1106);
    }

    #[test]
    fn op7_less_than() {
        // Opcode 7 - if the first parameter is less than the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.
        // Positional less than
        let test = vec![7, 7, 8, 9, 4, 9, 99, 10, 100, 999];
        let comp = IntcodeComputer::from_vec(test);
        test_output(&comp, 1);
        // Positional greater than
        let test = vec![7, 7, 8, 9, 4, 9, 99, 100, 10, 999];
        let comp = IntcodeComputer::from_vec(test);
        test_output(&comp, 0);
        // Positional equal
        let test = vec![7, 7, 8, 9, 4, 9, 99, 100, 100, 999];
        let comp = IntcodeComputer::from_vec(test);
        test_output(&comp, 0);

        // Immediate less than
        let test = vec![11107, 10, 100, 9, 4, 9, 99, -1, -1, 999];
        let comp = IntcodeComputer::from_vec(test);
        test_output(&comp, 1);
        // Immediate greater than
        let test = vec![11107, 100, 10, 9, 4, 9, 99, -1, -1, 999];
        let comp = IntcodeComputer::from_vec(test);
        test_output(&comp, 0);
        // Immediate equal
        let test = vec![11107, 100, 100, 9, 4, 9, 99, -1, -1, 999];
        let comp = IntcodeComputer::from_vec(test);
        test_output(&comp, 0);
    }

    #[test]
    fn op8_equals_than() {
        // Opcode 8 - if the first parameter is equal to the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.
        // Positional equals
        let test = vec![8, 7, 8, 9, 4, 9, 99, 100, 100, 999];
        let comp = IntcodeComputer::from_vec(test);
        test_output(&comp, 1);
        // Positional less than
        let test = vec![8, 7, 8, 9, 4, 9, 99, 10, 100, 999];
        let comp = IntcodeComputer::from_vec(test);
        test_output(&comp, 0);
        // Positional greater than
        let test = vec![8, 7, 8, 9, 4, 9, 99, 100, 10, 999];
        let comp = IntcodeComputer::from_vec(test);
        test_output(&comp, 0);

        // Immediate equals
        let test = vec![11108, 100, 100, 9, 4, 9, 99, -1, -1, 999];
        let comp = IntcodeComputer::from_vec(test);
        test_output(&comp, 1);
        // Immediate less than
        let test = vec![11108, 10, 100, 9, 4, 9, 99, -1, -1, 999];
        let comp = IntcodeComputer::from_vec(test);
        test_output(&comp, 0);
        // Immediate greater than
        let test = vec![11108, 100, 10, 9, 4, 9, 99, -1, -1, 999];
        let comp = IntcodeComputer::from_vec(test);
        test_output(&comp, 0);
    }

    #[test]
    fn position_mode_equal_to_8() {
        let test = vec![3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8];
        let base_comp = IntcodeComputer::from_vec(test);

        // Equal to 8 - expect 1
        test_input_output(&base_comp, 8, 1);

        // Greater than 8 - expect 0
        test_input_output(&base_comp, 9, 0);
        test_input_output(&base_comp, 7832, 0);

        // Less than 8 - expect 0
        test_input_output(&base_comp, 7, 0);
        test_input_output(&base_comp, -62398, 0);
    }

    #[test]
    fn position_mode_less_than_8() {
        let test = vec![3, 9, 7, 9, 10, 9, 4, 9, 99, -1, 8];
        let base_comp = IntcodeComputer::from_vec(test);

        // Less than 8 - expect 1
        test_input_output(&base_comp, 7, 1);
        test_input_output(&base_comp, -62719, 1);

        // Greater than - expect 0
        test_input_output(&base_comp, 9, 0);
        test_input_output(&base_comp, 324568, 0);

        // Equal to - expect 0
        test_input_output(&base_comp, 8, 0);
    }

    #[test]
    fn immediate_mode_equal_to_8() {
        let test = vec![3, 3, 1108, -1, 8, 3, 4, 3, 99];
        let base_comp = IntcodeComputer::from_vec(test);

        // Equal to 8 - expect 1
        test_input_output(&base_comp, 8, 1);

        // Greater than 8 - expect 0
        test_input_output(&base_comp, 9, 0);
        test_input_output(&base_comp, 72389, 0);

        // Less than - expect 0
        test_input_output(&base_comp, 7, 0);
        test_input_output(&base_comp, -62338, 0);
    }

    #[test]
    fn immediate_mode_less_than_8() {
        let test = vec![3, 3, 1107, -1, 8, 3, 4, 3, 99];
        let base_comp = IntcodeComputer::from_vec(test);

        // Less than 8 - expect 1
        test_input_output(&base_comp, 7, 1);
        test_input_output(&base_comp, -6341, 1);

        // Greater than 8 - expect 0
        test_input_output(&base_comp, 9, 0);
        test_input_output(&base_comp, 6342, 0);

        // Equal to - expect 0
        test_input_output(&base_comp, 8, 0);
    }

    #[test]
    fn position_mode_with_input() {
        let test = vec![3, 12, 6, 12, 15, 1, 13, 14, 13, 4, 13, 99, -1, 0, 1, 9];
        let base_comp = IntcodeComputer::from_vec(test);

        // Input 0 - expect 0
        test_input_output(&base_comp, 0, 0);

        // Greater than 0 - expect 1
        test_input_output(&base_comp, 1, 1);
        test_input_output(&base_comp, 5324, 1);

        // Less than 0 - expect 1
        test_input_output(&base_comp, -1, 1);
        test_input_output(&base_comp, -31241, 1);
    }

    #[test]
    fn immediate_mode_with_input() {
        let test = vec![3, 3, 1105, -1, 9, 1101, 0, 0, 12, 4, 12, 99, 1];
        let base_comp = IntcodeComputer::from_vec(test);

        // Input 0 - expect 0
        test_input_output(&base_comp, 0, 0);

        // Greater than - expect 1
        test_input_output(&base_comp, 1, 1);
        test_input_output(&base_comp, 1254, 1);

        // Less than - expect 1
        test_input_output(&base_comp, -1, 1);
        test_input_output(&base_comp, -1321, 1);
    }

    #[test]
    fn larger_example() {
        let test = vec![
            3, 21, 1008, 21, 8, 20, 1005, 20, 22, 107, 8, 21, 20, 1006, 20, 31, 1106, 0, 36, 98, 0,
            0, 1002, 21, 125, 20, 4, 20, 1105, 1, 46, 104, 999, 1105, 1, 46, 1101, 1000, 1, 20, 4,
            20, 1105, 1, 46, 98, 99,
        ];
        let base_comp = IntcodeComputer::from_vec(test);

        // Below 8 - expect 999
        test_input_output(&base_comp, 7, 999);
        test_input_output(&base_comp, -12, 999);

        // Equals 8 - expect 1000
        test_input_output(&base_comp, 8, 1000);

        // Greater than 8 - expect 1001
        test_input_output(&base_comp, 9, 1001);
        test_input_output(&base_comp, 10423, 1001);
    }

    fn test_input_output(base_comp: &IntcodeComputer, input: i32, expected_output: i32) {
        let mut test = base_comp.clone();
        test.inputs.push_back(input);
        test.process_intcodes();
        let result = test.outputs.pop_front().unwrap();
        assert_eq!(result, expected_output);
    }

    fn test_output(base_comp: &IntcodeComputer, expected_output: i32) {
        let mut test = base_comp.clone();
        test.process_intcodes();
        let result = test.outputs.pop_front().unwrap();
        assert_eq!(result, expected_output);
    }
}
