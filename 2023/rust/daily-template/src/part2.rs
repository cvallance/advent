use crate::custom_error::AocError;

pub fn process(_input: &str) -> miette::Result<u64, AocError> {
    let input = include_str!("../test_input.txt");
    Ok(0)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_process() -> miette::Result<()> {
        let input = include_str!("../test_input.txt");
        assert_eq!(0, process(input)?);
        Ok(())
    }
}
