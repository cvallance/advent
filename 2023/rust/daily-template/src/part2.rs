use crate::custom_error::AocError;

pub fn process(
    _input: &str,
) -> miette::Result<String, AocError> {
    todo!("day x - part 2");
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_process() -> miette::Result<()> {
        todo!("haven't built test yet");
        let input = include_str!("../test_input.txt");
        assert_eq!("", process(input)?);
        Ok(())
    }
}